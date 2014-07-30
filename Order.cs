using System;
using System.Collections.Generic;

namespace AmazonInterview
{
    #region entity
    public class Order
    {
        public long OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public float TotalCharge { get; set; } 
        public List<Product> OrderedProducts { get; set; }
        public List<ValidatorResult> ValidatorResult { get; set; }
        public bool Reject { get; set; } 
    }

    public class Customer
    {
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; } 
    }

    public class Product
    {
        public long ProductId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; } 
    }

    /// <summary>
    /// this class stores the validation result when a validator is applied to 
    /// an order
    /// </summary>
    public class ValidatorResult
    {
        public long ValidatorId { get; set; }
        public ValidatorCodeEnum Code { get; set; }
        public string Message { get; set; } 
    }

    public enum ValidatorCodeEnum
    {
        OK = 1, 
        InvalidQuantity = 2, 
        InvalidCharge = 3, 
        OutOfStock = 4,
        //...
    }
    #endregion

    #region logic
    /// <summary>
    /// individual validator need to implement this abstract class 
    /// </summary>
    public abstract class Validator
    {
        public int Id { get; set; } 

        abstract public ValidatorResult Validate(Order order);
    }

    /// <summary>
    /// this class provide an api to 
    /// 1. register validators with the system
    /// 2. apply validators to an order
    /// 3. reject the order if any validator reject the order
    /// </summary>
    public class OrderValidator
    {
        private Dictionary<int, Validator> _validators = new Dictionary<int,Validator>(); 

        /// <summary>
        /// function helps register the validator
        /// </summary>
        /// <param name="validator"></param>
        public void RegisterValidator(Validator validator) 
        {
            if (!_validators.ContainsKey(validator.Id))
            {
                _validators.Add(validator.Id, validator);
            }
        }

        /// <summary>
        /// function helps un-register a validator
        /// </summary>
        /// <param name="id"></param>
        public void RemoveValidator(int id)
        {
            if (_validators.ContainsKey(id))
                _validators.Remove(id);
        }

        /// <summary>
        /// function helps apply validators to the order
        /// </summary>
        /// <param name="order"></param>
        public void ApplyValidators(Order order)
        {
            if (order.ValidatorResult == null)
                order.ValidatorResult = new List<ValidatorResult>();

            //if there's no registered validator
            if (_validators.Count == 0) 
            {
                ValidatorResult result = new ValidatorResult() { Code = ValidatorCodeEnum.OK };
                order.ValidatorResult.Add(result);

                return;
            }

            foreach (Validator validator in _validators.Values)
            {
                ValidatorResult result = validator.Validate(order);
                order.ValidatorResult.Add(result);    
            }
        }

        /// <summary>
        /// function handles reject order
        /// </summary>
        /// <param name="order"></param>
        public void RejectOrder(Order order)
        {
            order.Reject = false;

            foreach (ValidatorResult result in order.ValidatorResult)
            {
                if (result.Code != ValidatorCodeEnum.OK)
                {
                    order.Reject = true;
                    break;
                }
            }

            if (order.Reject)
            {
                //logic to handle order rejection goes here
                //...
            }
        }
    }
    #endregion   
}
