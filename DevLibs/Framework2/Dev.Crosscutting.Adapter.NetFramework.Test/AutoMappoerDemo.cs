using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace Dev.Crosscutting.Adapter.NetFramework.Test
{

    namespace ConsoleApplication1
    {
        /// <summary>
        ///  From :http://www.cnblogs.com/ljzforever/archive/2011/12/29/2305500.html
        /// </summary>
        class Program
        {
            static void FF(string[] args)
            {
                //1.普通转换
                Name name1 = new Name() { FirstName = "L", LastName = "jz" };
                Mapper.CreateMap<Name, NameDto>()
                    .BeforeMap((name, nameDto) => Console.WriteLine("hello world before"))
                    .AfterMap((name, nameDto) => Console.WriteLine("hello world after"));
                NameDto nameDto1 = Mapper.Map<Name, NameDto>(name1);
                Console.WriteLine("1");
                Console.WriteLine(nameDto1.FirstName + nameDto1.LastName);
                Console.WriteLine();
                //Console.ReadKey();

                //整体设置
                //2.整体即时转换
                Mapper.Reset();
                Name name2 = new Name() { FirstName = "L", LastName = "jz" };
                Mapper.CreateMap<Name, NameDto>()
                    .ConstructUsing(name => new NameDto() { AllName = name.FirstName + name.LastName });
                NameDto nameDto2 = Mapper.Map<Name, NameDto>(name2);
                Console.WriteLine("2");
                Console.WriteLine(nameDto2.AllName);
                Console.WriteLine();
                //Console.ReadKey();

                //3.整体通过TypeConverter类型转换
                Mapper.Reset();
                Name name3 = new Name() { FirstName = "L", LastName = "jz" };
                Mapper.CreateMap<Name, NameDto>()
                    .ConvertUsing<NameConverter>();
                NameDto nameDto3 = Mapper.Map<Name, NameDto>(name3);
                Console.WriteLine("3");
                Console.WriteLine(nameDto3.AllName);
                Console.WriteLine();
                //Console.ReadKey();

                //单属性设置
                //4.属性条件转换
                Mapper.Reset();
                Name name4 = new Name() { FirstName = "L", LastName = "jz" };
                Mapper.CreateMap<Name, NameDto>()
                    .ForMember(name => name.FirstName, opt => opt.Condition(name => !name.FirstName.Equals("l", StringComparison.OrdinalIgnoreCase)));
                NameDto nameDto4 = Mapper.Map<Name, NameDto>(name4);
                Console.WriteLine("4");
                Console.WriteLine(string.IsNullOrEmpty(nameDto4.FirstName));
                Console.WriteLine();
                //Console.ReadKey();

                //5.属性忽略
                Mapper.Reset();
                Name name5 = new Name() { FirstName = "L", LastName = "jz" };
                Mapper.CreateMap<Name, NameDto>()
                    .ForMember(name => name.FirstName, opt => opt.Ignore());
                NameDto nameDto5 = Mapper.Map<Name, NameDto>(name5);
                Console.WriteLine("5");
                Console.WriteLine(string.IsNullOrEmpty(nameDto5.FirstName));
                Console.WriteLine();
                //Console.ReadKey();

                //6.属性转换
                Mapper.Reset();
                Name name6 = new Name() { FirstName = "L", LastName = "jz" };
                Mapper.CreateMap<Name, NameDto>()
                    .ForMember(name => name.AllName, opt => opt.MapFrom(name => name.FirstName + name.LastName));
                NameDto nameDto6 = Mapper.Map<Name, NameDto>(name6);
                Console.WriteLine("6");
                Console.WriteLine(nameDto6.AllName);
                Console.WriteLine();
                //Console.ReadKey();

                //7.属性通过ValueResolver转换
                Mapper.Reset();
                Name name7 = new Name() { FirstName = "L", LastName = "jz" };
                Mapper.CreateMap<Name, StoreDto>()
                    .ForMember(storeDto => storeDto.Name, opt => opt.ResolveUsing<NameResolver>());
                StoreDto store1 = Mapper.Map<Name, StoreDto>(name7);
                Console.WriteLine("7");
                Console.WriteLine(store1.Name.FirstName + store1.Name.LastName);
                Console.WriteLine();
                //Console.ReadKey();

                //8.属性填充固定值
                Mapper.Reset();
                Name name8 = new Name() { FirstName = "L", LastName = "jz" };
                Mapper.CreateMap<Name, NameDto>()
                    .ForMember(name => name.AllName, opt => opt.UseValue<string>("ljzforever"));
                NameDto nameDto8 = Mapper.Map<Name, NameDto>(name8);
                Console.WriteLine("8");
                Console.WriteLine(nameDto8.AllName);
                Console.WriteLine();
                //Console.ReadKey();

                //9.属性格式化
                Mapper.Reset();
                Name name9 = new Name() { FirstName = "L", LastName = "jz" };
                Mapper.CreateMap<Name, NameDto>()
                    .ForMember(name => name.FirstName, opt => opt.AddFormatter<StringFormatter>());
                NameDto nameDto9 = Mapper.Map<Name, NameDto>(name9);
                Console.WriteLine("9");
                Console.WriteLine(nameDto9.FirstName);
                Console.WriteLine();
                //Console.ReadKey();

                //10.属性null时的默认值
                Mapper.Reset();
                Name name10 = new Name() { FirstName = "L" };
                Mapper.CreateMap<Name, NameDto>()
                    .ForMember(name => name.LastName, opt => opt.NullSubstitute("jz"));
                NameDto nameDto10 = Mapper.Map<Name, NameDto>(name10);
                Console.WriteLine("10");
                Console.WriteLine(nameDto10.LastName);
                Console.WriteLine();
                //Console.ReadKey();

                //其它设置与特性
                //11.转换匿名对象
                Mapper.Reset();
                object name11 = new { FirstName = "L", LastName = "jz" };
                NameDto nameDto11 = Mapper.DynamicMap<NameDto>(name11);
                Console.WriteLine("11");
                Console.WriteLine(nameDto11.FirstName + nameDto11.LastName);
                Console.WriteLine();
                //Console.ReadKey();

                //12.转换DataTable
                //Mapper.Reset();
                //DataTable dt = new DataTable();
                //dt.Columns.Add("FirstName", typeof(string));
                //dt.Columns.Add("LastName", typeof(string));
                //dt.Rows.Add("L", "jz");
                //List<NameDto> nameDto12 = Mapper.DynamicMap<IDataReader, List<NameDto>>(dt.CreateDataReader());
                //Console.WriteLine("12");
                //Console.WriteLine(nameDto12[0].FirstName + nameDto12[0].LastName);
                //Console.WriteLine();
                //Console.ReadKey();
                ////emitMapper error
                ////List<NameDto> nameDto20 = EmitMapper.ObjectMapperManager.DefaultInstance.GetMapper<IDataReader, List<NameDto>>().Map(dt.CreateDataReader());


                //13.转化存在的对象
                Mapper.Reset();
                Mapper.CreateMap<Name, NameDto>()
                    .ForMember(name => name.LastName, opt => opt.Ignore());
                Name name13 = new Name() { FirstName = "L" };
                NameDto nameDto13 = new NameDto() { LastName = "jz" };
                Mapper.Map<Name, NameDto>(name13, nameDto13);
                //nameDto13 = Mapper.Map<Name, NameDto>(name13);//注意,必需使用上面的写法,不然nameDto13对象的LastName属性会被覆盖
                Console.WriteLine("13");
                Console.WriteLine(nameDto13.FirstName + nameDto13.LastName);
                Console.WriteLine();
                //Console.ReadKey();

                //14.Flatten特性
                Mapper.Reset();
                Mapper.CreateMap<Store, FlattenName>();
                Store store2 = new Store() { Name = new Name() { FirstName = "L", LastName = "jz" } };
                FlattenName nameDto14 = Mapper.Map<Store, FlattenName>(store2);
                Console.WriteLine("14");
                Console.WriteLine(nameDto14.NameFirstname + nameDto14.NameLastName);
                Console.WriteLine();
                //Console.ReadKey();

                //15.将Dictionary转化为对象,现在还不支持
                Mapper.Reset();
                Mapper.CreateMap<Dictionary<string, object>, Name>();
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("FirstName", "L");
                //Name name15 = Mapper.DynamicMap<Dictionary<string, object>, Name>(dict);
                Name name15 = Mapper.Map<Dictionary<string, object>, Name>(dict);
                Console.WriteLine("15");
                Console.WriteLine(name15.FirstName);
                Console.WriteLine();
                Console.ReadKey();
            }
        }


        public class Store
        {
            public Name Name { get; set; }
            public int Age { get; set; }
        }

        public class Name
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class StoreDto
        {
            public NameDto Name { get; set; }
            public int Age { get; set; }
        }

        public class NameDto
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string AllName { get; set; }
        }

        public class FlattenName
        {
            public string NameFirstname { get; set; }
            public string NameLastName { get; set; }
        }

        public class NameConverter : TypeConverter<Name, NameDto>
        {
            protected override NameDto ConvertCore(Name source)
            {
                return new NameDto() { AllName = source.FirstName + source.LastName };
            }
        }

        public class NameResolver : ValueResolver<Name, NameDto>
        {
            protected override NameDto ResolveCore(Name source)
            {
                return new NameDto() { FirstName = source.FirstName, LastName = source.LastName };
            }
        }

        public class NameFormatter : ValueFormatter<NameDto>
        {
            protected override string FormatValueCore(NameDto name)
            {
                return name.FirstName + name.LastName;
            }
        }

        public class StringFormatter : ValueFormatter<string>
        {
            protected override string FormatValueCore(string name)
            {
                return name + "-";
            }
        }
    }
}
