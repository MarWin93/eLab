using StructureMap;
using eWarsztaty.Domain;
using eWarsztaty.Web.Infrastructure;

namespace eWarsztaty.Web {
    public static class IoC {
        public static IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.Scan(scan =>
                                    {
                                        scan.TheCallingAssembly();
                                        scan.WithDefaultConventions();
                                    });
                              x.For<IWarsztatyDataSource>().HttpContextScoped().Use<eWarsztatyContext>();
                        });
            return ObjectFactory.Container;
        }
    }
}