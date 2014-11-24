UserTile
========

<a href="https://ci.appveyor.com/project/vbfox/usertile">
![Build status](https://ci.appveyor.com/api/projects/status/gtkd1w9ama9qdnko/branch/master)
</a>

This program is made to edit the Windows Vista and Windows 7 user "Tile" also called the "Profile Picture" by
modifying the binary blob stored under the registry key :

    HKEY_LOCAL_MACHINE\SAM\SAM\Domains\Account\Users

Using the program
-----------------

As this part of the registry is normally usable only by the system you will need
[SysInternals PsExec](http://technet.microsoft.com/en-us/sysinternals/bb897553) to execute it under the "Local System"
account. For example to list all users stored in the registry run as an administrator :

    psexec -S "C:\bin\UserTile.exe" --list

(All the path must be absolute)

Arguments
---------

### user, u

Must be specified for all commands that require an user name

### list, l

List the name of all users present in the registry

    UserTile --list

### export, e

Export the tile to a bitmap file

    UserTile --user vbfox --export C:\tile.bmp

### set, s

Set a new tile for the user. All the formats supported by the .Net framework are supported.
The image will be croped to a (centered) square, resized to 126x126 and converted to 16 bits per pixel colors.

    UserTile --user vbfox --set C:\tile.jpg

Thanks
------

I want to thank **Micah Rowland** and
[it's blog post on Xtreme Deployment](http://deployment.xtremeconsulting.com/2010/06/23/usertile-automation-part-1/)
for the first hints on where the tiles are stored and a basic analysis of the binary format.