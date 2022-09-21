#!/usr/bin/env bash
# This command copies the files on the other side of the soft link 
# WITHOUT copying any directories or duplicate softlinks or circular softlinks.
cp -Lr .soft_link_to_palettes/* ./
# Yes, using `.` in bash scripts is a no-no for both portability and maintainability, 
# but too bad.
