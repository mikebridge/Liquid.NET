#!/usr/bin/env ruby

#
# An informal scratch area 
#

require 'liquid'

def escape(str)
  return str.gsub('"','""')
end
  

def print_test(tmpl, vars = {})
  puts escape(tmpl);
  puts Liquid::Template.parse(tmpl).render(vars);
end

multiline_tmpl=<<HERE 
{% assign tests = "1,2,3,4" | split: "|" %}
{%
   for test in tests 
%}result:
{{
      test 
}}
{%
     endfor
   %}
HERE
print_test multiline_tmpl; # ///, { tests: [1,2,4,5] }

print_test "{% for test in tests %}result:{{ test }}{% endfor %}"; #, { tests: [1,2,4,5] }


print_test "foo {{ \"bar\" | upcase }}"

print_test "foo {{ \"hello\" | slice: -3,2 }}"

print_test "{% assign words = \"hello world\" | split: ' ' %} First Word: {{words.first}}"

print_test "Nothing: {{ \"xyz\" | truncate: 0 }}"
print_test "Nothing: {{ \"xyz\" | truncate: 2 }}"
print_test "Nothing: {{ \"xyz\" | truncate: 3 }}"
print_test "Nothing: {{ \"xyz\" | truncate: 4 }}"
print_test "Nothing: {{ \"xyz\" | truncate: 5 }}"
print_test "Nothing: {{ \"xyz\" | truncate: 6 }}"
print_test "Nothing: {{ \"abcd\" | truncate: 1 }}"
print_test "Nothing: {{ \"abcd\" | truncate: 2 }}"
print_test "Nothing: {{ \"abcd\" | truncate: 3 }}"
print_test "Nothing: {{ \"abcd\" | truncate: 4 }}"
print_test "Nothing: {{ \"abcd\" | truncate: 5 }}"
print_test "Nothing: {{ \"abcd\" | truncate: 6 }}"
print_test "Nothing: {{ \"abcd\" | truncate: 7 }}"


