using Controlleur.Classe;

namespace Controlleur.Interface
{
    interface IPublisher
    {
        void advertise(ITopic t, Format format);
        void publish(ITopic t, IPublication p);
        void unadvertise(ITopic t, Format format);
    }
}