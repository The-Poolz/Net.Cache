using System;
using Net.Cryptography.SHA256;
using Net.Web3.EthereumWallet;

namespace Net.Cache.DynamoDb.ERC20.DynamoDb.Models
{
    /// <summary>
    /// Represents a unique key that combines a blockchain chain identifier and an ERC20 token address.<br/>
    /// The key value is a SHA256 hash of the combined chain identifier and address.
    /// </summary>
    public class HashKey
    {
        /// <summary>
        /// Gets the blockchain network identifier.
        /// </summary>
        public long ChainId { get; }

        /// <summary>
        /// Gets the ERC20 token contract address.
        /// </summary>
        public EthereumAddress Address { get; }

        /// <summary>
        /// Gets the hashed representation of the <see cref="ChainId"/> and <see cref="Address"/> combination.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashKey"/> class.
        /// </summary>
        /// <param name="chainId">The blockchain network identifier.</param>
        /// <param name="address">The ERC20 token contract address.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="chainId"/> is less than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="address"/> is <c>null</c>.</exception>
        public HashKey(long chainId, EthereumAddress address)
        {
            if (chainId <= 0) throw new ArgumentOutOfRangeException(nameof(chainId));
            if (address == null) throw new ArgumentNullException(nameof(address));

            ChainId = chainId;
            Address = address;
            Value = Generate(chainId, address);
        }

        /// <summary>
        /// Generates a hashed key for the specified chain identifier and address.
        /// </summary>
        /// <param name="chainId">The blockchain network identifier.</param>
        /// <param name="address">The ERC20 token contract address.</param>
        /// <returns>A SHA256 hash representing the combined chain identifier and address.</returns>
        public static string Generate(long chainId, EthereumAddress address)
        {
            if (chainId <= 0) throw new ArgumentOutOfRangeException(nameof(chainId));
            if (address == null) throw new ArgumentNullException(nameof(address));

            return $"{chainId}-{address}".ToSha256();
        }

        /// <summary>
        /// Returns the hash key value.
        /// </summary>
        /// <returns>The hashed key string.</returns>
        public override string ToString() => Value;
    }
}
