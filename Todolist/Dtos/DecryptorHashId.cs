using HashidsNet;

namespace Todolist.Dtos
{
    public static class DecryptorHashId
    {
        public const string Key = "-1T3^nohQ)RU|N`";
        public const string UseSymbols = "abcdefghijklmnopqrstuvwyxyz1234567890";

        public static Hashids HashFactory()
        {
            var hashids = new Hashids(
                Key,
                minHashLength: 8,
                alphabet: UseSymbols);

            return hashids;
        }
    }
}
