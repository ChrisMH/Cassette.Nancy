# @reference "~/Scripts/lib"

window.app = window.app ? {}
window.app.layout = window.app.layout ? {}

$header = null
$content = null
$footer = null

onResize = () ->
  width = $(window).width() - ($header.outerWidth(true) - $header.width())

  $header.css
    position: 'absolute'
    display: 'block'
    left: 0
    top: 0
    width: width
    
  top = $header.outerHeight(true)

  height = $(window).height() - top - $footer.outerHeight(true) - ($content.outerHeight(true) - $content.height())
  width = $(window).width() - ($content.outerWidth(true) - $content.width())

  $content.css
    position: 'absolute'
    display: 'block'
    left: 0
    top: top
    width: width
    height: height

  top += $content.outerHeight(true)
  width = $(window).width() - ($footer.outerWidth(true) - $footer.width())

  $footer.css
    position: 'absolute',
    display: 'block',
    left: 0,
    top: top,
    width: width

  # Trigger the resize event on the content
  $content.trigger('resize', [])

window.app.layout.init = (headerSel, contentSel, footerSel) ->
  $header = $(headerSel)
  $content = $(contentSel)
  $footer = $(footerSel)
  onResize()
  $(window).bind('resize', _.debounce(onResize, 25))
