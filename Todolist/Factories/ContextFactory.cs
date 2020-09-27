
namespace Todolist.Factories
{
    public class ContextFactory : IContextFactory
    {
        public Context GetContext()
        {
            return new Context();
        }
    }
}