namespace Backend.Util
{
    public class Converter
    {
        public static TTarget Convert<TTarget>(object source)
            where TTarget : new()
        {
            TTarget target = new TTarget();
            var sourceProperties = source.GetType().GetProperties();
            var targetProperties = typeof(TTarget).GetProperties();
            foreach (var sourceProp in sourceProperties)
            {
                var targetProp = targetProperties
                    .FirstOrDefault(p => p.Name == sourceProp.Name && p.PropertyType == sourceProp.PropertyType);
                if (targetProp != null && targetProp.CanWrite)
                {
                    var value = sourceProp.GetValue(source);
                    targetProp.SetValue(target, value);
                }
            }
            return target;
        }
        public static List<TTarget> Convert<TTarget>(IEnumerable<object> source)
            where TTarget : new()
        {
            return source.Select(Convert<TTarget>).ToList();
        }
    }
}
