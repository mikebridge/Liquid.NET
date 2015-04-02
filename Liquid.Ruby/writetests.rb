#!/usr/bin/env ruby

require 'liquid'
home = File.join(File.dirname(__FILE__), '..')

test_template = File.read('tests.liquid')

def escape(str)
  str.gsub('"','\"')
end

def eval_tmpl(tmpl, vars = {})
  #escape(tmpl);
  begin
    return Liquid::Template.parse(tmpl).render(vars);
  rescue Liquid::SyntaxError => err
    return err.message
  end


end

def test_line(tmpl)
  { "input" => escape(tmpl), "expected" => eval_tmpl(tmpl)}
end

# FILTER TESTS

filters = {
    'classname' => 'FilterTests',
    'tests' => [
    test_line("{{ \"3\" | times: \"3\" }}")
]}

filter_output = File.join(home, 'Liquid.NET.Tests/Ruby/FilterTests.cs')
File.open(filter_output, 'w') {
   |file| file.write(eval_tmpl(test_template, filters))
}

# ERROR TESTS

filters = {
    'classname' => 'ErrorTests',
    'tests' => [
        test_line("{{ \"1\" | divided_by: \"0\" }}"),
        test_line("{{ \"x\" | divided_by: \"1\" }}"),
        test_line('{{ 1 | unk_filter }}'),
        test_line("{{ \"test,test\" | split: }}"),
        test_line('{% unknown_tag %}')
]}

filter_output = File.join(home, 'Liquid.NET.Tests/Ruby/ErrorTests.cs')
File.open(filter_output, 'w') {
    |file| file.write(eval_tmpl(test_template, filters))
}


#puts filters
#puts eval_tmpl(test_template, filters)

#
# filters = { 'errortests' => [
#     { "input" => escape("{{ \"\" | times: \"3\" }}"), "expected" => escape('9') }
# ]}
#
# test_template = File.read('tests.liquid')
# filter_output = File.join(home, 'Liquid.NET.Tests/Ruby/ErrorTests.cs')
# File.open(filter_output, 'w') {
#     |file| file.write(write_test_file(test_template, filters))
# }
# #puts write_test_file(test_template, filters)

