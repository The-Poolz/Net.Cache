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
using Nethereum.Contracts.QueryHandlers.MultiCall;
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

        /// <inheritdoc cref="IErc20Service.GetErc20TokenAsync"/>
        public async Task<Erc20TokenData> GetErc20TokenAsync(EthereumAddress token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));

            var multiCallFunction = new Aggregate3Function
            {
                Calls = new List<Call3>
                {
                    new Call3 { Target = token, CallData = new NameFunction().GetCallData() },
                    new Call3 { Target = token, CallData = new SymbolFunction().GetCallData() },
                    new Call3 { Target = token, CallData = new DecimalsFunction().GetCallData() },
                    new Call3 { Target = token, CallData = new TotalSupplyFunction().GetCallData() }
                }
            };
            
            var handler = _web3.Eth.GetContractQueryHandler<Aggregate3Function>();
            var response = await handler
                .QueryAsync<Aggregate3OutputDTO>(_multiCall, multiCallFunction)
                .ConfigureAwait(false);
            var responseValidator = new MultiCallResponseValidator(multiCallFunction.Calls.Count);
            var validation = await responseValidator
                .ValidateAsync(response.ReturnData.Select(x => x.ReturnData).ToArray())
                .ConfigureAwait(false);
            if (!validation.IsValid)
            {
                var error = string.Join(" ", validation.Errors.Select(e => e.ErrorMessage));
                throw new Erc20QueryException(token, error);
            }

            var name = response.ReturnData[0].ReturnData.Decode<NameOutputDTO>().Name;
            var symbol = response.ReturnData[1].ReturnData.Decode<SymbolOutputDTO>().Symbol;
            var decimals = response.ReturnData[2].ReturnData.Decode<DecimalsOutputDTO>().Decimals;
            var supply = response.ReturnData[3].ReturnData.Decode<TotalSupplyOutputDTO>().TotalSupply;

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
