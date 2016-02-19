
## Coding Process on YosemitCloud
#### Clone code
> gitpull.sh ${progectname} ${dir} 例如：gitpull.sh common/orm

> ${dir}可以为空,默认为当前路径 ${progectname}为项目名称，名称可从 https://git.yosemitecloud.com中获取。

> gitpull.sh （仅限第一次拉代码时使用）

#### Check out new branch
> `git checkout -b robinworking master`

#### Modify code
```
git add
git status
git commit
```

#### Test and Verify
> `git merge master feature    # merge the master branch to feature branch, but will have an extraneous merge commit.`



#### Code Review
> `gitpush.sh`

### Merge to Master
```BASH

```
## Git Usage Reference
### Create working branch
```
git checkout -b robinworking master
git branch
git branch -remote
git revert HEAD                撤销前一次 commit
git revert HEAD^               撤销前前一次 commit
git revert commit-id          （比如：fa042ce57ebbe5bb9c8db709f719cec2c58ee7ff）撤销指定的版本，撤销也会作为一次提交进行保存。
git revert                     提交一个新的版本，将需要revert的版本的内容再反向修改回去，版本会递增，不影响之前提交的内容
git reset --hard HEAD~3        会将最新的3次提交全部重置，就像没有提交过一样
git reset <file>               发现错误的将不想提交的文件add进入index之后，想回退取消
git rm --cached README.txt     将文件状态还原为未暂存状态，即回到“Untracked files”文件状态
git fetch --all                只是下载远程的库的内容，不做任何的合并
git reset --hard origin/master 把HEAD指向刚刚下载的最新的版本
```
#### Git Path
##### Standard diff and patch
```
1. git diff > PATCH.diff
2. git apply PATCH.diff
```
##### Git format patch
`This can be send to other guys to patch`
1.

### Branch
To delete a local branch
`git branch -d the_local_branch`
