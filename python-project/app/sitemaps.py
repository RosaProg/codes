from django.contrib.sitemaps import Sitemap 

class ExportAbroadSitemap(Sitemap):
	changefreq = "weekly"
	priority = 0.5

	def __init__(self, names):
		self.names = names

	def items (self):
		return self.names