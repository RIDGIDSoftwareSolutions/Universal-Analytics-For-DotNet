using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace UniversalAnalyticsHttpWrapper.Objects
{
    /// <summary>
    /// Data object holding a GUID representing a client id (or cid).
    /// See https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters#cid
    /// </summary>
    public class ClientId
    {
        // Randomly generated UUID chosen as the 'namespace' for generating deterministic GUIDs from strings for use as 
        // client ids. 
        private static readonly Guid CidNamespace = new Guid("174bb9db-f7df-4503-9302-56c6d58e53d2");

        /// <summary>
        /// Creates a new client id with a random Guid as the id.
        /// </summary>
        public ClientId()
        {
            this.Id = Guid.NewGuid();
        }

        /// <summary>
        /// Creates a new client id with a guid generated from a seed string.
        /// </summary>
        public ClientId(string value)
        {
            // Use a version 5 UUID generated from the SHA1 hash of 'value' 
            // to create the cid. 

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            byte[] namespaceBytes = CidNamespace.ToByteArray();

            // GUID struct stores time_low, time_mid, and time_hi_and_version (see RFC 4122) 
            // as DWORD and WORD(s), respectively. Network byte order 
            // requires big endian for these groups but they are stored in little 
            // endian on x86 machines. Swap those byte groups: 
            SwapByteOrder(namespaceBytes);

            // Concatenate the namespace with the 'name' (valueBytes) for hashing. 
            byte[] concatBytes = namespaceBytes.Concat(valueBytes).ToArray();

            // Hash the concatenated byte array. 
            byte[] hash;
            using (SHA1 sha1 = new SHA1CryptoServiceProvider())
            {
                hash = sha1.ComputeHash(concatBytes);
            }

            byte[] cidBytes = new byte[16]; // 128-bytes for a new GUID. 
            Buffer.BlockCopy(hash, 0, cidBytes, 0, 16); // Copy the first 128-bytes of the hash (160 bytes) to the new GUID. 

            // The current GUID is in network byte order; this is big-endian. So, the MSB of 
            // time_hi_and_version is index 6 not 7. 
            cidBytes[6] &= 0x5F;

            // Set msb of clock_seq_hi_and_reserved to zero and one, respectively. 
            cidBytes[8] &= 0x7F;

            // Convert back to local byte order (little-endian): 
            SwapByteOrder(cidBytes);

            Id = new Guid(cidBytes);
        }

        private static void SwapByteOrder(byte[] guid)
        {
            // time_low, DWORD 
            Swap(0, 3, guid);
            Swap(1, 2, guid);

            // time_mid, WORD 
            Swap(5, 4, guid);

            // time_hi_and_version, WORD 
            Swap(7, 6, guid);
        }

        private static void Swap(int a, int b, byte[] arr)
        {
            byte temp = arr[a];
            arr[a] = arr[b];
            arr[b] = temp;
        }

        /// <summary>
        /// Creates a new client id with the supplied Guid 'id' as the client id.
        /// </summary>
        /// <param name="id">The Guid representing the client id.</param>
        /// <exception cref="ArgumentNullException">Thrown if 'id' is null.</exception>
        public ClientId(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.Id = id;
        }

        /// <summary>
        /// Value holder for the client id.
        /// </summary>
        public Guid Id { get; }
    }
}