namespace Domo.DI
{
    public enum LifeStyle
    {
        Default,
        Transient,
        Singleton,
        Scoped,

        //ThreadLocal,
        //HttpContext,
        //HttpSession,
        //Hybrid // See StructureMap
    }
}