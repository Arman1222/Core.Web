using AutoMapper;
using System;
using System.Linq.Expressions;

namespace Core.Web.Utilities
{
    //http://stackoverflow.com/questions/4987872/ignore-mapping-one-property-with-automapper
    public static class AutoMapperHelper
    {
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> map,
        Expression<Func<TDestination, object>> selector)
            {
                map.ForMember(selector, config => config.Ignore());
                return map;
            }
    }
}
