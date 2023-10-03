using Controlleur.Interface;

namespace Controlleur.Classe
{
    class Publisher : IPublisher
    {

        void IPublisher.advertise(ITopic t, IPublication.Format format)
        {
            throw new NotImplementedException();
        }


        void IPublisher.publish(ITopic t, IPublication p)
        {
            throw new NotImplementedException();
        }


        void IPublisher.unadvertise(ITopic t, IPublication.Format format)
        {
            throw new NotImplementedException();
        }
    }
}