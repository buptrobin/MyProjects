## Git Usage

![Git Data Transfer Commands](http://blog.osteele.com/images/2008/git-transport.png)
![Git Workflow](http://blog.osteele.com/images/2008/git-workflow.png)
### Revert changes

> git checkout -- <file> 取消对文件的修改。还原到最近的版本，废弃本地做的修改。
>
> git reset HEAD <file>... 取消已经暂存的文件。即，撤销先前"git add"的操作
>
> git commit --amend 修改最后一次提交。用于修改上一次的提交信息，或漏提交文件等情况。
>
> git reset HEAD^ 回退所有内容到上一个版本
>
> git reset HEAD^ a.py 回退a.py这个文件的版本到上一个版本
>
> git reset –soft HEAD~  向前回退到第1个版本
> ![Git reset soft](https://git-scm.com/images/reset/reset-soft.png)
>
> git reset –mixed HEAD~

> ![Git reset mixed](https://git-scm.com/images/reset/reset-mixed.png)
>
> git reset --hard HEAD~

> ![Git reset hard](https://git-scm.com/images/reset/reset-hard.png)

> git reset –hard origin/master 将本地的状态回退到和远程的一样

>
> git reset 057d 回退到某个版本
>
> git revert HEAD 回退到上一次提交的状态，按照某一次的commit完全反向的进行一次commit.(代码回滚到上个版本，并提交git)
>

| |head	|index work |dirwd |safe|
|---|---|---|---|---|
Commit Level| | | | |	 |
reset --soft [commit]|	REF|	NO|	NO|	YES|
reset [commit] |	REF|	YES|	NO|	YES|
reset --hard [commit]	| REF|	YES|	YES|	NO|
checkout [commit]	|HEAD	|YES|	YES|	YES|
File Level| | | | | |	 |
reset (commit) [file]	|NO|	YES|	NO|	YES|
checkout (commit) [file]|	NO|	YES|	YES|	NO|

#### Not push yet
+ Git reset --mixed 会保留源码,只是将git commit和index 信息回退到了某个版本.
```
git reset 默认是 --mixed 模式
git reset --mixed  等价于  git reset
```

+ Git reset --soft 保留源码,只回退到commit 信息到某个版本.不涉及index的回退,如果还需要提交,直接commit即可.
+ Git reset --hard 源码也会回退到某个版本,commit和index 都回回退到某个版本.(注意,这种方式是改变本地代码仓库源码)
```
源码也会回退到某个版本,commit和index 都回回退到某个版本.(注意,这种方式是改变本地代码仓库源码)
当然有人在push代码以后,也使用 reset --hard <commit...> 回退代码到某个版本之前,但是这样会有一个问题,你线上的代码没有变,线上commit,index都没有变,当你把本地代码修改完提交的时候你会发现权是冲突.....
```

#### Pushed
对于已经把代码push到线上仓库,你回退本地代码其实也想同时回退线上代码,回滚到某个指定的版本,线上,线下代码保持一致.你要用到下面的命令

+ git revert是用一次新的commit来回滚之前的commit，git reset是直接删除指定的commit
+

### Git Stash
>git stash: 备份当前的工作区的内容，从最近的一次提交中读取相关内容，让工作区保证和上次提交的内容一致。同时，将当前的工作区内容保存到Git栈中。

>git stash pop: 从Git栈中读取最近一次保存的内容，恢复工作区的相关内容。由于可能存在多个Stash的内容，所以用栈来管理，pop会从最近的一个stash中读取内容并恢复。

>git stash list: 显示Git栈内的所有备份，可以利用这个列表来决定从那个地方恢复。

>git stash clear: 清空Git栈。此时使用gitg等图形化工具会发现，原来stash的哪些节点都消失了。

>git stash apply stash@{1} 就可以将你指定版本号为stash@{1}的工作取出来

>git format-patch -n , n是具体某个数字， 例如 'git format-patch -1' 这时便会根据log生成一个对应的补丁，如果 'git format-patch -2' 那么便会生成2个补丁，当然前提是你的log上有至少有两个记录

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
#### Git Patch
##### Standard diff and patch
```
1. git diff > PATCH.diff
2. git apply PATCH.diff
```
##### Git format patch
`This can be send to other guys to patch`


### Branch
To delete a local branch
`git branch -d the_local_branch`



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
