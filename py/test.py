#!/usr/bin/env python
from __future__ import print_function, unicode_literals

import sys
import os
sys.path.append('..')
import renpy.object
import renpy.config
import renpy.loader
import renpy.translation
import renpy.translation.generation



try:
    import renpy.util
except:
    pass
class RenPyArchive:
    file = None
    handle = None

    files = {}
    indexes = {}

    def __init__(self, file, index):
        self.load(file, index)

    # Converts a filename to archive format.
    def convert_filename(self, filename):
        (drive, filename) = os.path.splitdrive(os.path.normpath(filename).replace(os.sep, '/'))
        return filename


    # List files in archive and current internal storage.
    def list(self):
        return list(self.indexes)

    # Read file from archive or internal storage.
    def read(self, filename):
        filename = self.convert_filename(filename)
        if(filename != '.' and isinstance(self.indexes[filename], list)):
            if hasattr(renpy.loader, "load_from_archive"):
                subfile =  renpy.loader.load_from_archive(filename)
            else:
                subfile =  renpy.loader.load_core(filename)
            return subfile.read()
        else: return None

    # Load archive.
    def load(self, filename, index):
        self.file = "game/"+filename
        self.files = {}
        self.indexes = {}
        self.handle = open(self.file, 'rb')
        base, ext = filename.rsplit(".", 1)
        renpy.config.archives.append(base)
        renpy.config.searchpath = [os.path.dirname(os.path.realpath(self.file))]
        renpy.config.basedir = os.path.dirname(renpy.config.searchpath[0])
        renpy.loader.index_archives()
        items = renpy.loader.archives[index][1].items()
        for file, index in items:
            self.indexes[file] = index

if __name__ == "__main__":
    import argparse


    #for filename in translate_list_files():
        #    write_translates(filename, args.language, filter)

    parser = argparse.ArgumentParser(
        description='A tool for working with Ren\'Py archive files.',
        epilog='The FILE argument can optionally be in ARCHIVE=REAL format, mapping a file in the archive file system to a file on your real file system. An example of this: rpatool -x test.rpa script.rpyc=/home/foo/test.rpyc',
        add_help=False)

    parser.add_argument('-r',action="store_true", dest='remove', help='Delete archives after unpacking.')
    parser.add_argument('dir',type=str, help='The Ren\'py dir to operate on.')
    arguments = parser.parse_args()
    directory = arguments.dir
    os.chdir(directory)
    remove = arguments.remove
    output = 'game'
    archive_extentions = []
    if hasattr(renpy.loader, "archive_handlers"):
        for handler in renpy.loader.archive_handlers:
            for ext in handler.get_supported_extensions():
                if ext not in archive_extentions:
                    archive_extentions.append(ext)
    else: archive_extentions.append('.rpa')
    archives = []
    for root, dirs, files in os.walk(directory):
        for file in files:
            try:
                base, ext = file.rsplit('.', 1)
                if '.'+ext in archive_extentions:
                    archives.append(file)
            except:
                pass
    if archives != []:
        for arch in archives:
            print("  Unpacking \"{0}\" archive.".format(arch))
            # try:
            archive = RenPyArchive(arch, archives.index(arch))

            files = archive.list()
            renpy.config.translate_files = files
            #filesnames = renpy.translation.translate_list_files()
            filesnames = renpy.translation.generation.translate_list_files()

            #filenames = renpy.translation.generation.write_translates()
            print(files)

            # Create output directory if not present.
            if not os.path.exists(output):
                os.makedirs(output)


            # Iterate over files to extract.
            for filename in files:
                outfile = filename
                contents = archive.read(filename)
                #if(None != contents):
                    # Create output directory for file if not present.
                    #if not os.path.exists(os.path.dirname(os.path.join(output, outfile))):
                    #    os.makedirs(os.path.dirname(os.path.join(output, outfile)))

                    #with open(os.path.join(output, outfile), 'wb') as file:
                    #    file.write(contents)
            # except Exception as err:
            #     print(err)
            #     sys.exit(1)
        print("  All archives unpaked.")
        if remove:
            for archive in archives:
                print("  Archive {0} has been deleted.".format(archive))
                os.remove("{0}{1}".format(directory, archive))
    else:
        print("  There are no archives in the game folder.")