#### Color Paletts, Easily Imported From GIMP!
Color paletts in GIMP are just ASCII text of RGB values. These numbers can be pulled into pretty much whatever you want.
#### How to Use
1. Find the *palettes* folder of your personl GIMP directory: Open GIMP > Edit > Preferences > Expand the Folders Tab > Palettes > ask yourself why in the world GIMP decided to put this in such a weird directory?
2. Create a soft link to there: `ln -s <souce> .soft_link_to_palettes`
3. Pull in all files from there: `./resync.bash`
