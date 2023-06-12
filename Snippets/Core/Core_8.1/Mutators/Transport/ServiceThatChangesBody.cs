namespace Core8.Mutators.Transport
{
    public static class ServiceThatChangesBody
    {
        public static byte[] Mutate(byte[] body)
        {
            return null;
        }

        public static byte[] Mutate(object message)
        {
            return null;
        }

    }
}