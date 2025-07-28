using System;
using Nethereum.Web3;
using Nethereum.Contracts;
using System.Threading.Tasks;
using Net.Web3.EthereumWallet;
using System.Collections.Generic;
using Net.Cache.DynamoDb.ERC20.RPC.Models;
using Net.Cache.DynamoDb.ERC20.RPC.Extensions;
using Net.Cache.DynamoDb.ERC20.RPC.Exceptions;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;

namespace Net.Cache.DynamoDb.ERC20.RPC
{
    public class Erc20Service : IErc20Service
    {
        private readonly IWeb3 _web3;
        private readonly EthereumAddress _multiCall;

        public Erc20Service(IWeb3 web3, EthereumAddress multiCall)
        {
            _web3 = web3 ?? throw new ArgumentNullException(nameof(web3));
            _multiCall = multiCall ?? throw new ArgumentNullException(nameof(multiCall));
        }

        public Erc20Token GetEr20Token(EthereumAddress token)
        {
            return GetEr20TokenAsync(token).GetAwaiter().GetResult();
        }

        public async Task<Erc20Token> GetEr20TokenAsync(EthereumAddress token)
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
            var response = await handler.QueryAsync<List<byte[]>>(_multiCall, multiCallFunction);

            // TODO: Include FluentValidation lib to validate result
            if (response.Count != multiCallFunction.Calls.Length) throw new Erc20QueryException(token, "MultiCall returned unexpected number of results.");
            if (response.Exists(r => r == null || r.Length == 0))
                throw new Erc20QueryException(token, "One of ERC20 calls failed (empty return).");

            var name = response[0].Decode<NameOutputDTO>().Name;
            var symbol = response[1].Decode<SymbolOutputDTO>().Symbol;
            var decimals = response[2].Decode<DecimalsOutputDTO>().Decimals;
            var supply = response[3].Decode<TotalSupplyOutputDTO>().TotalSupply;

            return new Erc20Token(token, name, symbol, decimals, supply);
        }
    }
}
