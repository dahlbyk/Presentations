using AutoMapper;

namespace Interfaces
{
    public interface IMapperProfile
    {
        void Initialize(IProfileExpression mapper);
    }
}