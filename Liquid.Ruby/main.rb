#!/usr/bin/env ruby

#
# An informal scratch area 
#

require 'liquid'

var1 = 'test'

def escape(str)
  return str.gsub('"','""')
end
  

def print_test(tmpl, vars = {})
  puts escape(tmpl);
  puts Liquid::Template.parse(tmpl).render(vars);
end

# multiline_tmpl=<<HERE
# {% assign tests = "1,2,3,4" | split: "|" %}
# {%
#    for test in tests
# %}result:
# {{
#       test
# }}
# {%
#      endfor
#    %}
# HERE
# print_test multiline_tmpl; # ///, { tests: [1,2,4,5] }

# print_test "{% for test in tests %}result:{{ test }}{% endfor %}"; #, { tests: [1,2,4,5] }


# print_test "foo {{ \"bar\" | upcase }}"

# print_test "foo {{ \"hello\" | slice: -3,2 }}"

# print_test "{% assign words = \"hello world\" | split: ' ' %} First Word: {{words.first}}"

# print_test "{{ \"hello\" | times: 3 }}"

# print_test "{{ 3 | times: 3 }}"

# print_test "{{ \"3\" | plus : 3 }}"

# print_test "{{ 3 | plus : \"3\" }}"

# print_test "{{ \"3\" | plus : \"3\" }}"

# print_test "{{ \"test\" | plus : \"test\" }}"

# print_test "{{ \"test\" | capitalize }}"

# print_test "{{ \"test\" | times : 3 }}"

# print_test "{{ 3 | times : 3 }}"

#print_test "{{ assign a = \"\" | split: ' ' }}{% for x in a %}HELLO{% else %}NOTHING {% endfor %}"

#print_test "Nothing: {{ \"xyz\" | truncate: 0 }}"
#print_test "Nothing: {{ \"xyz\" | truncate: 2 }}"
#print_test "Nothing: {{ \"xyz\" | truncate: 3 }}"
#print_test "Nothing: {{ \"xyz\" | truncate: 4 }}"
#print_test "Nothing: {{ \"xyz\" | truncate: 5 }}"
#print_test "Nothing: {{ \"xyz\" | truncate: 6 }}"
#print_test "Nothing: {{ \"abcd\" | truncate: 1 }}"
#print_test "Nothing: {{ \"abcd\" | truncate: 2 }}"
#print_test "Nothing: {{ \"abcd\" | truncate: 3 }}"
#print_test "Nothing: {{ \"abcd\" | truncate: 4 }}"
#print_test "Nothing: {{ \"abcd\" | truncate: 5 }}"
#print_test "Nothing: {{ \"abcd\" | truncate: 6 }}"
#print_test "Nothing: {{ \"abcd\" | truncate: 7 }}"

print_test "{% assign a = '' %}{% decrement 'a' %}"
print_test "{% assign a = port %}{% decrement port %}"
print_test "{% assign a = '' %}{% decrement a %}"
print_test "{% assign a = 3 %}{% decrement a %}{% decrement a %}"
print_test "{% decrement a %}{% decrement a %}{% increment a %}{% decrement b %}"
#print_test "{{ missing }}"
