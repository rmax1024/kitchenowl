import 'package:flutter/material.dart';

class SelectableButtonListTile extends StatefulWidget {
  final String title;
  final IconData? icon;
  final String? description;
  final bool selected;
  final bool raised;
  final void Function()? onPressed;
  final void Function()? onLongPressed;
  final Widget? extraOption;

  const SelectableButtonListTile({
    super.key,
    required this.title,
    this.icon,
    this.description,
    required this.selected,
    this.onPressed,
    this.onLongPressed,
    this.raised = true,
    this.extraOption,
  });

  @override
  State<SelectableButtonListTile> createState() =>
      _SelectableButtonListTileState();
}

class _SelectableButtonListTileState extends State<SelectableButtonListTile> {
  bool mouseHover = false;

  @override
  Widget build(BuildContext context) {
    return Card(
      margin: const EdgeInsets.symmetric(vertical: 2),
      elevation: !widget.raised ? 0 : null,
      color: !widget.raised
          ? ElevationOverlay.applySurfaceTint(
              Theme.of(context).colorScheme.background,
              Theme.of(context).colorScheme.surfaceTint,
              1.5,
            )
          : Theme.of(context).colorScheme.primary,
      child: MouseRegion(
        onEnter: (event) {
          setState(() {
            mouseHover = true;
          });
        },
        onExit: (event) {
          setState(() {
            mouseHover = false;
          });
        },
        child: ListTile(
          leading: widget.selected
              ? const Icon(Icons.check_rounded)
              : widget.icon != null
                  ? Icon(widget.icon,
                      color: !widget.raised
                          ? Theme.of(context).iconTheme.color!.withAlpha(85)
                          : Theme.of(context).iconTheme.color!.withAlpha(170))
                  : null,
          title: Text(
            widget.title + ((widget.description?.isNotEmpty ?? false) ? (' - ' + widget.description!) : ''),
            maxLines: 1,
            overflow: TextOverflow.ellipsis,
            style: Theme.of(context).textTheme.bodyMedium!.copyWith(
                color: !widget.raised
                    ? null
                    : Theme.of(context)
                    .colorScheme
                    .onPrimary
                    .withOpacity(.9)),
          ),
          selected: widget.selected,
          visualDensity: VisualDensity(vertical: -4),
          onTap: widget.onPressed,
          onLongPress: widget.onLongPressed,
          contentPadding: const EdgeInsets.only(left: 16, right: 8),
          trailing: (widget.extraOption != null && mouseHover)
              ? widget.extraOption
              : (widget.onLongPressed != null && mouseHover)
                  ? IconButton(
                      onPressed: widget.onLongPressed,
                      color: widget.raised
                          ? Theme.of(context).colorScheme.onPrimary
                          : null,
                      icon: const Icon(Icons.more_horiz_rounded),
                    )
                  : null,
        ),
      ),
    );
  }
}
