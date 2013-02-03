namespace Domo.DI.Activation
{
    public enum Scope
    {
        Default,
        Transient,
        Singleton,

        //ThreadLocal,
        //HttpContext,
        //HttpSession,
        //Hybrid // See StructureMap
    }
}