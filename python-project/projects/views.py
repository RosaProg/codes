#projects/views.py
from __future__ import absolute_import

from django.contrib.auth.models import User
from django.contrib.auth.decorators import login_required
from django.http import JsonResponse
from django.shortcuts import render, redirect

from company.models import Product

from .models import Customer, Contact, Attachment, Post
from .forms import CustomerForm, ContactForm, AttachmentForm, PostForm


@login_required
def customers(request):
	try:
		user = request.user.id 
		customers = Customer.objects.filter(user=request.user.id)
		context_dict = {'customers':customers}
	except Customer.DoesNotExist:	
		context_dict = {}
	return render(request, 'projects/customers.html', context_dict)


@login_required
def new_customer(request):
	"UPS adds a new customer"
	if request.method == "POST":
		form = CustomerForm(request.POST)
		if form.is_valid():
			new_customer = form.save(commit=False)
			new_customer.user = request.user
			new_customer.save()
			return redirect('/projects/')
		else:
			print form.errors
	else:
		form = CustomerForm()
	context_dict={'form':form}
	return render(request, 'projects/new_customer.html', context_dict)





@login_required
def customer_detail(request, slug):
	customer = Customer.objects.get(slug=slug)
	contacts = Contact.objects.filter()
	my_files = Attachment.objects.filter(owner=request.user)
	products = Product.objects.filter(user=request.user)
	context_dict = {'my_files': my_files, 
					'customer': customer,
					'products': products}
	return render(request, 'projects/customer_detail.html', context_dict)



@login_required
def add_file(request):
	if request.method == 'POST':
		form = AttachmentForm(request.POST, request.FILES)
		if form.is_valid():
			user = request.user
			new_file = form.save(commit=False)
			new_file.owner = user
			new_file.save()
			return redirect(projects)
		else:
			print form.errors
	else:
		form = AttachmentForm()

	context_dict = {'form':form}
	return render(request, 'projects/add_file.html', context_dict)

@login_required
def create_post(request):
    if request.method == 'POST':
        form = PostForm(request.POST)
        post_text = request.POST.get('the_post')
        response_data = {}
        post = Post(text=post_text, author=request.user)
        post.save()

        response_data['result'] = 'Create post successful!'
        response_data['postpk'] = post.pk
        response_data['text'] = post.text
        response_data['created'] = post.created.strftime('%B %d, %Y %I:%M %p')
        response_data['author'] = post.author.username

        return JsonResponse(response_data)
    else:
        return JsonResponse({"nothing to see": "this isn't happening"})