using DentiCare.Models.Treatments;
using DentiCare.Repository.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DentiCare.Controllers
{
    public class TreatmentsController : BaseController
    {
        private ITreatmentsRepository _treatmentsRepository;

        public TreatmentsController(ITreatmentsRepository treatmentsRepository) :base()
        {
            _treatmentsRepository = treatmentsRepository;
        }

        #region Prices

        [HttpGet]
        public ActionResult Prices()
        {
            var categories = GetCategoriesQuery();
            var treatments = GetPricesQuery();

            var model = new List<PriceByCategoryViewModel>();
            foreach (var category in categories)
            {
                model.Add(new PriceByCategoryViewModel
                {
                    CategoryName = category.Name,
                    Treatments = treatments.Where(x => x.Category == category.Name).ToList()
                });
            }

            return View(model);
        }

        #endregion

        #region Helper methods

        private List<TreatmentPrice> GetPricesQuery()
        {
            var query = from t in _treatmentsRepository.Treatments
                        join c in _treatmentsRepository.TreatmentCategories
                        on t.category_id equals c.id
                        orderby t.name
                        select new TreatmentPrice
                        {
                            Name = t.name,
                            Category = c.name,
                            Price = t.price
                        };

            return query.ToList();
        }
        private List<Category> GetCategoriesQuery()
        {
            var categories = from c in _treatmentsRepository.TreatmentCategories
                             orderby c.name
                             select new Category
                             {
                                 ID = c.id,
                                 Name = c.name
                             };

            return categories.ToList();
        }
        
        #endregion
    }
}