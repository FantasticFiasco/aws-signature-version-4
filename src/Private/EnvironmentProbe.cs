using System;

namespace AwsSignatureVersion4.Private
{
    public class EnvironmentProbe
    {
        private readonly Lazy<bool> isMono;

        public EnvironmentProbe()
        {
           isMono = new Lazy<bool>(() => Type.GetType("Mono.Runtime") != null);
        }

        public virtual bool IsMono => isMono.Value;
    }
}
