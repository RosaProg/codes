import os
os.environ.setdefault("DJANGO_SETTINGS_MODULE", "exportAbroad.settings")

import django
django.setup()

from newmarkets.models import Product, Country, ImportData

PATH = '/home/hakim/Downloads/Data'
ALL_THE_FILES = [file for path, dirs, files in os.walk(PATH) for file in files]

def populate()




if __name__ == '__main__':
	main()