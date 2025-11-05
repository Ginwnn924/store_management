namespace StoreManagement.Mapper
{
    public interface IMapper<TModel, TDto> where TModel : class 
                                            where TDto : class
    {
        TDto ToDto(TModel entity);

        TModel ToModel(TDto dto);

        IEnumerable<TDto> ToDtoList(IEnumerable<TModel> entities);

        IEnumerable<TModel> ToModelList(IEnumerable<TDto> dtos);

        void MapToExistingModel(TDto dto, TModel entity);
    }
}
