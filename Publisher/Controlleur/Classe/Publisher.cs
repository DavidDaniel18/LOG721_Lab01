using Controlleur.Interface;

namespace Controlleur.Classe
{
    class Publisher : IPublisher
    {

        void IPublisher.advertise(ITopic t, Format format)
        {
            throw new NotImplementedException();
        }


        void IPublisher.publish(ITopic t, IPublication p)
        {
            throw new NotImplementedException();
        }


        void IPublisher.unadvertise(ITopic t, Format format)
        {
            throw new NotImplementedException();
        }
    }
}