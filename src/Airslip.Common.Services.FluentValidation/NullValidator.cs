using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Models;
using FluentValidation;
using FluentValidation.Results;
using System.Threading.Tasks;

namespace Airslip.Common.Services.FluentValidation
{
    public class NullValidator<TType> : ModelValidatorBase<TType> 
        where TType : class, IModel
    {
        
    }
}