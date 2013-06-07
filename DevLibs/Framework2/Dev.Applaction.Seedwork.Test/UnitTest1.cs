using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;
namespace Dev.Applaction.Seedwork.Test
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Dev.Crosscutting.Adapter;
    using Dev.Crosscutting.Adapter.Adapter;
    using Dev.Crosscutting.Adapter.NetFramework.Adapter;


    //public static class ProjectionsExtensionMethods2
    //{
    //    /// <summary>
    //    /// Project a type using a DTO
    //    /// </summary>
    //    /// <typeparam name="TProjection">The dto projection</typeparam>
    //    /// <param name="entity">The source entity to project</param>
    //    /// <returns>The projected type</returns>
    //    public static TProjection ProjectedAs<TProjection>(this AEntity item)
    //        where TProjection : class,new()
    //    {
    //        var adapter = TypeAdapterFactory.CreateAdapter();
    //        return adapter.Adapt<TProjection>(item);
    //    }
    //    /// <summary>
    //    /// projected a enumerable collection of items
    //    /// </summary>
    //    /// <typeparam name="TProjection">The dtop projection type</typeparam>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="items">the collection of entity items</param>
    //    /// <returns>Projected collection</returns>
    //    public static IEnumerable<TProjection> ProjectedAsCollection<TProjection>(this IEnumerable<AEntity> items)
    //        where TProjection : class,new()
    //    {

    //        var adapter = TypeAdapterFactory.CreateAdapter();

    //        return adapter.Adapt<List<TProjection>>(items);
    //    }

    //    public static bool Pro(IEnumerable<AEntity> items)
    //    {

    //        return true;
    //    }
    //}

    public class AEntity
    {
        public string S { get; set; }

    }

    public class BEntity
    {
        public string S1 { get; set; }
    }

    public class ADTO
    {
        public string S { get; set; }
        public string S1 { get; set; }
    }





    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void init()
        {
            TypeAdapterFactory.SetCurrent(new AutomapperTypeAdapterFactory());



        }
        [TestMethod]
        public void TestMethod1()
        {
            AutoMapper.Mapper.CreateMap<AEntity, ADTO>();
            var s = new AEntity();
            s.S = "1;";

            var bb = s.ProjectedAs<ADTO>();

            Assert.AreEqual(bb.S, s.S);
        }

        [TestMethod]
        public void TestMethod2()
        {
            AutoMapper.Mapper.CreateMap<AEntity, ADTO>()
                      .ForMember(dto => dto.S1, mc => mc.MapFrom(e => e.S))
                      .ReverseMap();
            ;
            var entity = new AEntity();
            entity.S = "1;";

            var bb = entity.ProjectedAs<ADTO>();

            Assert.AreEqual(bb.S, entity.S);
            Assert.AreEqual(entity.S, bb.S1);
        }


        [TestMethod]
        public void TestMethod2_list()
        {
            AutoMapper.Mapper.CreateMap<AEntity, ADTO>()
                      .ForMember(dto => dto.S1, mc => mc.MapFrom(e => e.S))
                //.ReverseMap()
                      ;
            ;
            AEntity[] entity = new AEntity[]
                             {
                                 new AEntity{ S ="1"}
                             };

            IEnumerable<AEntity> fasdfasdf = entity.AsEnumerable();

            //var list = fasdfasdf.ProjectedAsCollection<ADTO>();


            var list = fasdfasdf.ProjectedAsCollection<ADTO>();

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count() > 0);
            Assert.AreEqual("1", list.ElementAt(0).S);



            //fasdfasdf.
        }


        [TestMethod]
        public void TestMethod2_list_reverse()
        {
            AutoMapper.Mapper.CreateMap<AEntity, ADTO>()
                      .ForMember(dto => dto.S1, mc => mc.MapFrom(e => e.S))
                .ReverseMap()
                      ;
            var mydto = new ADTO { S1 = "2", S = "1" };


            var entity = mydto.ProjectedAs<AEntity>();

            Assert.AreEqual("1", entity.S);


            var mydto2 = entity.ProjectedAs<ADTO>();

            Assert.AreEqual("1", mydto2.S1);

            //fasdfasdf.
        }


        [TestMethod]
        public void TestMethod2_2_Maper()
        {
            AutoMapper.Mapper.CreateMap<AEntity, ADTO>()
                      .ForMember(dto => dto.S1, mc => mc.MapFrom(e => e.S))
                .ReverseMap()
                      ;

            AutoMapper.Mapper.CreateMap<BEntity, ADTO>()

                .ReverseMap()
                      ;
            var mydto = new ADTO { S1 = "2", S = "1" };


            var entity = mydto.ProjectedAs<AEntity>();

            Assert.AreEqual("1", entity.S);

            var mydto2 = entity.ProjectedAs<ADTO>();

            Assert.AreEqual("1", mydto2.S1);


            var entity2 = mydto.ProjectedAs<BEntity>();

            Assert.AreEqual("2", entity2.S1);

            var mydto3 = entity2.ProjectedAs<ADTO>();

            Assert.AreEqual("2", mydto3.S1);
           

            //fasdfasdf.
        }







    }



}
