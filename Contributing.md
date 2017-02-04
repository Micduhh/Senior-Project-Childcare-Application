# Contributing to the Project - Basic Information
Based on the nature of Github, and our past experiences, we have decided that the best way for us to contribute code to the project
is by enforcing a stricter version control system.  Based on that fact, we will have to adopt a branching pattern that will
suit us.

## The Branching Pattern
The application will consist of two main branches: the main branch, and the development branch.

### The Development Branch
The development branch is the "work-in-progress" branch.  This is where we will pull all of the latest changes in regard
to team contribution.  All pull requests will come to this branch, and all checkouts will come from this branch.

### The Master Branch
This is the "final" version of the application.  This branch should only contain the version of the application that we
would give to the client. 

# Code Contribution Process
To contribute to the program, please follow the following steps to insure that the latest changes are pushed up, and ensuring
that there are no merge conflicts.  Do note that you don't have to follow this particular rule-set, nor do you have to do it
in any particular order, but if we want to minimize the chance of merge conflicts, this is the best approach to take.

## File an Issue
If there is a feature that we would like to add to the project, or a bug fix, or just refactoring, go to the Issues tab and
file an issue there so that we can all see what the issue is, and what needs to be done.  Make the title short and to-the-point,
and make the description as descriptive as possible.

## Fork
If you would like to work on an issue that was posted on the Issue tab, take ownership of the issue and then Fork the project.  When you
create a Fork, you are free to change the code base as you please to fix the designated issue.  When pushing, you are free to push it
to your own master at will.  You do not need group consent for this.

## Pull Request
When you feel that the issue has been fixed, update your forks master and then create a Pull Request to the Development branch on
the main projects repository.  Please have a brief, yet concise title, along with a description of what was worked on.

### Warning
DO NOT PUSH STRAIGHT TO THE DEVELOPMENT BRANCH

Since the development branch is where we get all of our working copies from, we need it to be as secure as possible.  Only use a Pull Request to get the changes there.

DO NOT ACCEPT YOUR OWN PULL REQUEST TO THE DEVELOPMENT BRANCH ON THE MAIN REPOSITORY.  If you are working on your own fork, you can do as you please in regards to pushing to your own local branches.  But, please do not push to the development branch until everyone has looked at it and agrees on the Pull Request.

## Pushing to the Master branch
The master branch contains the version of the application that we would consider "final".  This is the version of the application that we would eventually give our client.  Since it is the main version of the app, we need to lock it down really, really, really, REALLY hard.  We will NEVER push to this branch UNTIL we have the CONSENT OF EVERYONE.  In order to do this, we need to ensure that the development branch is in the state that we want it in.  The branch should have all of the features that the client has requested.  Once we have come to an agreement on pushing to master, we shall do so.

## More Information soon
