// ***********************************************************************************
// Created by zbw911 
// �����ڣ�2012��12��18�� 13:28
// 
// �޸��ڣ�2013��02��18�� 18:24
// �ļ�����IEFFetchingRepository.cs
// 
// ����и��õĽ����������ʼ���zbw911#gmail.com
// ***********************************************************************************

using System.Data.Entity.Core.Objects.DataClasses;

namespace Kt.Framework.Repository.Data.EntityFramework
{
    public interface IEFFetchingRepository<TEntity, TFetch> : IRepository<TEntity> where TEntity : EntityObject
    {
        EFRepository<TEntity> RootRepository { get; }

        string FetchingPath { get; }
    }
}