using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.Intercepting
{
    public class LoggingWrapper : IInterceptionBehavior
    {
        class Program
        {
            static void Main(string[] args)
            {
                // Declara o container normalmente
                var container = new UnityContainer();

                // Informa ao container que ele irpa usar interceptção
                container.AddNewExtension<Interception>();

                // Registra o IDevice e adiciona um interceptador à ele (LoggingWrapper)
                container.RegisterType<IDevice, Device>(
                    new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<LoggingWrapper>());

                // Pede uma instancia de IDevice para o container.
                var device = container.Resolve<IDevice>();
                device.Open();
                device.Close();

                // Veja que o sistema logou as chamas sem precisar espalhar logs pelo projeto.
                Console.ReadLine();
            }
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            // Before invoking the method on the original target.
            WriteLog($"Invoking method {input.MethodBase} at {DateTime.UtcNow}");

            // Invoke the next behavior in the chain.
            var result = getNext()(input, getNext);

            // After invoking the method on the original target.
            WriteLog(result.Exception != null
                ? $"Method {input.MethodBase} threw exception {result.Exception.Message} at {DateTime.Now.ToLongTimeString()}"
                : $"Method {input.MethodBase} returned {result.ReturnValue} at {DateTime.Now.ToLongTimeString()}");

            return result;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute => true;

        private void WriteLog(string message)
        {
            Console.WriteLine(message);
        }
    }
    public interface IDevice
    {
        void Open();
        void SendCommand(int code);
        void Close();
    }

    public class Device : IDevice
    {
        private readonly StreamWriter _serialPort;

        public Device()
        {
            _serialPort = new StreamWriter("PortStub.txt");
        }

        public void Open()
        {
            SendCommand(0x16);
        }

        public void SendCommand(int code)
        {
            _serialPort.Write($"Eat the command {code}!");
        }

        public void Close()
        {
            SendCommand(0x32);
        }
    }

   
}
