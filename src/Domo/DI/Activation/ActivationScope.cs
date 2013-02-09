namespace Domo.DI.Activation
{
    public enum ActivationScope
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