static class ReflectionHelper
{
    public static T GetPropertyValue<T>(this object entityInstance, string property)
    {
        return (T)entityInstance.GetType().GetProperty(property).GetValue(entityInstance);
    }
}