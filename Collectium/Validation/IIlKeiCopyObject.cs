namespace Collectium.Validation
{
    public interface IIlKeiCopyObject
    {
        public IIlKeiCopyObject WithSource(Object obj);
        public IIlKeiCopyObject WithDestination(Object obj);
        public IIlKeiCopyObject Exclude(string obj);
        public IIlKeiCopyObject Include(string obj);

        public void Execute();
    }
}
