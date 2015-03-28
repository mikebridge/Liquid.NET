#!/usr/bin/env ruby

require 'liquid'

def escape(str)
  return str.gsub('"','""')
end
  

def print_test(tmpl)
  puts escape(tmpl);
  puts Liquid::Template.parse(tmpl).render
end

print_test "foo {{ \"bar\" | upcase }}"

print_test "foo {{ \"hello\" | slice: -3,2 }}"

print_test "{% assign words = \"hello world\" | split: ' ' %} First Word: {{words.first}}"
