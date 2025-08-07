using System;
using System.Linq;
using Nethereum.Web3;
using Nethereum.Contracts;
using System.Threading.Tasks;
using Net.Web3.EthereumWallet;
using System.Collections.Generic;
using Net.Cache.DynamoDb.ERC20.Rpc.Exceptions;
using Net.Cache.DynamoDb.ERC20.Rpc.Extensions;
using Net.Cache.DynamoDb.ERC20.Rpc.Models;
using Net.Cache.DynamoDb.ERC20.Rpc.Validators;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;

namespace Net.Cache.DynamoDb.ERC20.Rpc
{
    /// <summary>
    /// Retrieves ERC20 token information via blockchain RPC calls.
    /// </summary>
    public class Erc20Service : IErc20Service
    {
        private readonly IWeb3 _web3;
        private readonly EthereumAddress _multiCall;

        /// <summary>
        /// Initializes a new instance of the <see cref="Erc20Service"/> class.
        /// </summary>
        /// <param name="web3">The web3 client used for RPC communication.</param>
        /// <param name="multiCall">The address of the multicall contract.</param>
        public Erc20Service(IWeb3 web3, EthereumAddress multiCall)
        {
            _web3 = web3 ?? throw new ArgumentNullException(nameof(web3));
            _multiCall = multiCall ?? throw new ArgumentNullException(nameof(multiCall));
        }

        /// <inheritdoc cref="IErc20Service.GetErc20Token"/>
        public Erc20TokenData GetErc20Token(EthereumAddress token)
        {
            return GetErc20TokenAsync(token).GetAwaiter().GetResult();
        }

        /// <inheritdoc cref="IErc20Service.GetErc20TokenAsync"/>
        public async Task<Erc20TokenData> GetErc20TokenAsync(EthereumAddress token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            var multiCallFunction = new MultiCallFunction(
                calls: new[]
                {
                    new MultiCall(to: token, data: new NameFunction().GetCallData()),
                    new MultiCall(to: token, data: new SymbolFunction().GetCallData()),
                    new MultiCall(to: token, data: new DecimalsFunction().GetCallData()),
                    new MultiCall(to: token, data: new TotalSupplyFunction().GetCallData())
                }
            );

            var handler = _web3.Eth.GetContractQueryHandler<MultiCallFunction>();
            var response = await handler
                .QueryAsync<List<byte[]>>(_multiCall, multiCallFunction)
                .ConfigureAwait(false);
            var responseValidator = new MultiCallResponseValidator(multiCallFunction.Calls.Length);
            var validation = await responseValidator
                .ValidateAsync(response)
                .ConfigureAwait(false);
            if (!validation.IsValid)
            {
                var error = string.Join(" ", validation.Errors.Select(e => e.ErrorMessage));
                throw new Erc20QueryException(token, error);
            }

            var name = response[0].Decode<NameOutputDTO>().Name;
            var symbol = response[1].Decode<SymbolOutputDTO>().Symbol;
            var decimals = response[2].Decode<DecimalsOutputDTO>().Decimals;
            var supply = response[3].Decode<TotalSupplyOutputDTO>().TotalSupply;

            var tokenResult = new Erc20TokenData(token, name, symbol, decimals, supply);
            var tokenValidator = new Erc20TokenValidator();
            var tokenValidation = await tokenValidator
                .ValidateAsync(tokenResult)
                .ConfigureAwait(false);
            if (!tokenValidation.IsValid)
            {
                var error = string.Join(" ", tokenValidation.Errors.Select(e => e.ErrorMessage));
                throw new Erc20QueryException(token, error);
            }

            return tokenResult;
        }
    }
}
