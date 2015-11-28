using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DailyPriceNotifier_Decorator.Notifiers
{
    public abstract class ExternalNotifierDecorator : INotifier
    {
        //protected INotifier BaseNotifier; // Notifier being decorated
        protected INotifier BaseNotifier;

        public ExternalNotifierDecorator(INotifier notifier)
        {
            this.BaseNotifier = notifier;
        }
        public virtual string Message { get { return BaseNotifier.Message; } set { BaseNotifier.Message = value; } }

        public virtual void Notify()
        {
            BaseNotifier.Notify();
        }

        public virtual void OnSuccessAppendLog(string componentName)
        {
            BaseNotifier.OnSuccessAppendLog(componentName);
        }

        public virtual int LoggedCount()
        {
            return BaseNotifier.LoggedCount();
        }

        public INotifier RemoveDecorator<T>(T toRemove) where  T : Type
        {
            // Can't remove the base concrete component - SimpleNotifier
            // It does not implement ExternalNotifierDecorator class
            if (!typeof (ExternalNotifierDecorator).IsAssignableFrom(toRemove))
            {
                return this;
            }

            if (this.GetType().Equals(toRemove))
            {
                return this.BaseNotifier;
            }
            else if(this.BaseNotifier is ExternalNotifierDecorator)
            {
                ExternalNotifierDecorator baseDecorator = (ExternalNotifierDecorator)this.BaseNotifier;
                this.BaseNotifier = baseDecorator.RemoveDecorator<T>(toRemove);
                return this;
            }

            // next notifier is concrete component - SimpleNotifier
            // so this is the end of the chain
            return this.BaseNotifier;
        }

        public bool HasDecorator<T>(T typeToFind) where T : Type
        {
            // Type that is not derived from ExternalNotifierDecorator could not be in chain
            if (!typeof(ExternalNotifierDecorator).IsAssignableFrom(typeToFind))
            {
                return false;
            }
            if (this.GetType().Equals(typeToFind)) return true;
            else if (this.BaseNotifier is ExternalNotifierDecorator)
            {
                // If element's next element is decorator, let's inspect him
                ExternalNotifierDecorator baseDecorator = (ExternalNotifierDecorator)this.BaseNotifier;
                return baseDecorator.HasDecorator<T>(typeToFind);
            }

            // If type is decorator
            // but is not the type to find and it has no next decorators
            // means that this is the end of the chain - no occurrences found
            return false;
        }

        public INotifier AddDecorator<T>(T typeToAdd) where T : Type
        {
            // Type that is not derived from ExternalNotifierDecorator could not be in chain
            if (!typeof(ExternalNotifierDecorator).IsAssignableFrom(typeToAdd))
            {
                return this;
            }

            // typeToAdd is decorator, so lets create instance
            return (INotifier)Activator.CreateInstance(typeToAdd, this);
        }

    }
}
