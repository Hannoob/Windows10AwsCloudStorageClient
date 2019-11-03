# Windows 10 AWS S3 cloud storage app

A basic cloud storage app utilizing AWS S3

## Why build this app

This app is really the first project that I did that utilized AWS services for a non-trivial problem.
The idea was that I have a need to store data in the cloud, more than what was given away for free by existing cloud storage providers, but also a lot less than the 1TB or 2TB bundles one is sometimes forced to purchase.
This is when I realized that I could use AWS S3 with a nice client app to mimic the basic behaviour of something like the other cloud storage providers at a fraction of the cost, since I will only pay for the storage that I actually end up using.
Getting the app up and running ended up being quite a simple task, given that I did not need to do any fancy authentication steps (Although if there is a better way to do this, I am all ears).

## How to get it set up

Getting it set up is quite a manual process at this point since I have not built the cloudformation scripts to automate any of the aws resources.

### Step 1 - Download and build the code

This should be fairly straight forward for anyone with VisualStudio 2019 installed.

### Step 2 - Create the bucket in AWS

Log into your AWS account (oo create one if you don't have one already) and navigate to the S3 service.
Once in S3 create a new bucket:

![Creating a bucket](https://github.com/Hannoob/windows10-aws-cloud-storage-client/blob/master/images/S3.bmp)

You can configure the bucket to your requirements.
I just added a tag to mine, and made sure that it is not publicly accessible.

### Step 3 - Create a user that will be interacting with the bucket

In this step, you will create an IAM user that will have permissions to read and write to this newly created bucket.
I am not going to go into depth on this one, as I am just using my dev account to interact with the bucket. (Not good practice I know, but this is just for me)

After the user is created with the appropriate permissions, export the AccessKeyId and the SecretAccessKey.
This will be used when configuring the app.

### Step 4 - Configure the app

Now that your cloud environment is set up, build and run the app.
You should see a small cloud icon pop up in your system tray.

![Tray Icon](https://github.com/Hannoob/windows10-aws-cloud-storage-client/blob/master/images/tray-icon.bmp)

If you right-click on the icon, the app should display a little menu.
Click on the settings option.

![Tray icon menu](https://github.com/Hannoob/windows10-aws-cloud-storage-client/blob/master/images/tray-icon-click.bmp)

The settings form should show in the bottom right corner of the screen:

![Settings menu](https://github.com/Hannoob/windows10-aws-cloud-storage-client/blob/master/images/settings.bmp)

Simply add all the config that you have prepared in the corresponding fields.

### Step 5 - Test it out

Now you should be able to test out your new cloud storage solution.
You can click on the icon and select "Open Folder", or you can right click and select "Open Folder" from the menu.

![Open folder](https://github.com/Hannoob/windows10-aws-cloud-storage-client/blob/master/images/open-folder.bmp)

This should open the folder that you have configured on the settings form.

![Cloud folder](https://github.com/Hannoob/windows10-aws-cloud-storage-client/blob/master/images/cloud-folder.bmp)

Add an item here and check whether it is synced to your aws account:

![Bucket file](https://github.com/Hannoob/windows10-aws-cloud-storage-client/blob/master/images/s3-folder.bmp)

## Things to keep in mind

Keep in mind that the app in its current form, does not cater for syncing to multiple computers so be very careful of making changes while offline or from different machines.
This could potentially overwrite some of the other changes that you have made.
I would also not recommend using this for anything important, because I am not sure how well this will hold up under stress.

## Conclusion

All in all this was a fun little project to work on, and a super useful one at that.
One of the things that I struggled with a lot was actually getting my user access to the bucket, I might amend this quick setup tutorial to include a detailed description of how to create a user with the correct access and permissions.
I hope that you enjoy using your own cloud storage, as much as I enjoyed writing it.

P.S. There is still a LOT of fixes required to make this thing robust, and any contributions would be greatly appreciated!
