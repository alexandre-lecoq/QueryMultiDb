# Important notes regarding icon generations

Read this very carefully if you intend to generate a new ICO file.

## SVG file

Ideally, we would have an SVG file and generate de ICO file directly from it using a script.

However, there are 2 problems :

* We don't have such a script
* The SVG file drawing is not the same as the ICO file drawing

A white fill was added to every PNG files generated from the SVG file.

## Generation process



Steps to follow:

1. Convert the SVG to several PNG files (1 for each resolution)
1.1. Resolutions : 16, 24, 32, 48, 64, 96, 128, 192, 256, 512
2. Fill the icon with white color
3. Convert all PNG files to one ICO file

