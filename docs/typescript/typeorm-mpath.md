# 更新树实体的父级实体时，没有更新 mpath 的问题临时解决方案

> 该问题已得到官方修复，[ISSUES](https://github.com/typeorm/typeorm/issues/2418)

```ts
export async function fixMPath<TKey, TEntity>(
  id: TKey,
  parentId: TKey | null | undefined,
  entityRepository: TreeRepository<TEntity>,
  entityManager?: EntityManager,
): Promise<void> {
  const entity = await entityRepository.findOne(id);
  if (!entity) {
    return;
  }

  const entityMpath = await this.getEntityMpath(entityRepository, id);
  const parentMpath = parentId && (await this.getEntityMpath(entityRepository, parentId));

  const newMpath = `${parentMpath}${id}.`;

  const entityWithChildren = await entityRepository.findDescendants(entity);
  const toUpdate = entityWithChildren.map((p) => p['id']);

  const entityMetadata = entityRepository.metadata;
  const query = `UPDATE ${entityMetadata.tableName} SET mpath = REPLACE(mpath, '${entityMpath}', '${newMpath}')  WHERE ${entityMetadata.tableName}.id IN (${toUpdate.join(',')})`;
  if (entityManager) {
    await entityManager.query(query);
  } else {
    await entityRepository.query(query);
  }
}

private static async getEntityMpath<TKey, TEntity>(entityRepository: TreeRepository<TEntity>, id: TKey): Promise<string> {
  const {
    mpath,
  } = await entityRepository
    .createQueryBuilder()
    .where('id = :id', { id: id })
    .select('mpath')
    .getRawOne();

  if (!mpath) {
    throw new InternalServerErrorException('fix mpath failed,entity or mpath not found.');
  }

  return mpath;
}
```