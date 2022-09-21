using AutoMapper;

//http://stackoverflow.com/questions/5737505/automapper-error-on-simple-conversion-without-createmap
namespace Core.Web.Utilities
{
    public interface IMyMapper<T1, T2>
    {
        T2 Map(T1 source);

        T2 Map(T1 source, T2 dest);
    }

    public class MyMapper<T1, T2> : IMyMapper<T1, T2>
        where T1 : class
        where T2 : class
    {
        public MyMapper()
        {
            Mapper.CreateMap<T1, T2>();
        }

        public T2 Map(T1 source)
        {
            return Mapper.Map<T1, T2>(source);
        }

        public T2 Map(T1 source, T2 dest)
        {
            return Mapper.Map<T1, T2>(source, dest);
        }
    }

    //public class MyMapper<IList<T1>, IList<T2>> : IMyMapper<IList<T1>, IList<T2>>
    //    where T1 : class
    //    where T2 : class
    //{
    //    public MyMapper()
    //    {
    //        Mapper.CreateMap<T1, T2>();
    //    }

    //    public IList<T2> Map(IList<T1> source)
    //    {
    //        return Mapper.Map<IList<T1>, IList<T2>>(source);
    //    }

    //    public IList<T2> Map(IList<T1> source, IList<T2> dest)
    //    {
    //        return Mapper.Map<IList<T1>, IList<T2>>(source, dest);
    //    }
    //}
}
