# cat_cafe

## To contribute (workflow)

We are using the feature branch workflow ([details here](https://www.atlassian.com/git/tutorials/comparing-workflows/feature-branch-workflow), or see the summary below)

### 1 - Sync with the remote 

Make sure you're working with the latest version of the project
```
git checkout master
git fetch origin 
git reset --hard origin/master
```

### 2 - Create a new branch

Give your new branch a name referring to an issue (or maybe a group of similar issues)
```
git checkout -b new-feature
```

Regularly, you might want to get all the new code from your master (yeah, we forgot to rename it "main", sorry) branch, to work with an up-to-date codebase:
```
git pull --rebase origin master
```

### 3 - Code

:fire::technologist::bug::fire:............:white_check_mark:

### 4 - Save your changes to your new branch

For a refresher, see details about `add`, `commit`, `push`, etc. [here](https://www.atlassian.com/git/tutorials/saving-changes)  

It should involve creating a corresponding feature branch on the remote repository
```
git push -u origin branch-name-that-describes-the-new-feature
```

### 5 - Create a Pull Request

On [the repository's main page](https://codefirst.iut.uca.fr/git/alexis.drai/dice_app), or on your new branch's master page, look for a `New Pull Request` button.  

It should then allow you to `merge into: ...:master` and `pull from: ...:new-feature`  

Follow the platform's instructions, until you've made a "work in progress" (WIP) pull request. You can now assign reviewers among your colleagues. They will get familiar with your new code -- and will either accept the branch as it is, or help you arrange it.