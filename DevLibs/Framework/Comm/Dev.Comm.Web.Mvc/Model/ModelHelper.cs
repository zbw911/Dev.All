using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Dev.Comm.Web.Mvc.Model
{
    /// <summary>
    /// 模型帮助方法
    /// </summary>
    public class ModelHelper
    {
        /// <summary>
        /// 直接取得Model
        /// </summary>
        /// <param name="controller"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static TModel GetModel<TModel>(Controller controller) where TModel : class
        {
            return GetModel<TModel>(controller, false);
        }

        /// <summary>
        /// 直接取得Model
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="ignoreModelStateError"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        public static TModel GetModel<TModel>(Controller controller, bool ignoreModelStateError) where TModel : class
        {
            TModel model = (TModel)DependencyResolver.Current.GetService(typeof(TModel));

            if (model == null)
            {
                throw new ArgumentNullException("model");
            }


            var controllerContext = controller.ControllerContext;

            var valueProvider = ValueProviderFactories.Factories.GetValueProvider(controllerContext);

            IModelBinder binder = ModelBinders.Binders.GetBinder(typeof(TModel));

            var innerModelState = new ModelStateDictionary();

            ModelBindingContext bindingContext = new ModelBindingContext()
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, typeof(TModel)),
                //ModelName = prefix,
                ModelState = innerModelState,
                //PropertyFilter = propertyFilter,
                ValueProvider = valueProvider
            };




            var obj = binder.BindModel(controllerContext, bindingContext);

            var error = Dev.Comm.Web.Mvc.Model.ModelStateHandler.GetAllError(innerModelState);

            if (error.Count() > 0)
            {
                throw new InvalidCastException(string.Join(",", error.Select(x => x.ErrorMessage)));
            }

            return obj as TModel;

        }
    }
}
