using System.Linq;
using Todolist.Dtos;

namespace Todolist.Extensions
{
    public static class Extensions
    {
        public static string EncryptId(this int id)
        {
            var hashids = DecryptorHashId.HashFactory();
            var result = hashids.EncodeLong(id);
            return result;
        }

        public static long DecodeLongByHashIds(this string hash)
        {
            var hashIds = DecryptorHashId.HashFactory();
            var result = hashIds.DecodeLong(hash).FirstOrDefault();
            return result;
        }
    }
}