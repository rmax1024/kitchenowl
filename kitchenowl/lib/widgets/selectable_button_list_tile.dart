import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:kitchenowl/cubits/shoppinglist_cubit.dart';
import 'package:kitchenowl/cubits/item_edit_cubit.dart';
import 'package:kitchenowl/enums/shoppinglist_sorting.dart';
import 'package:kitchenowl/models/item.dart';
import 'package:collection/collection.dart';
import 'package:kitchenowl/models/shoppinglist.dart';
import 'package:kitchenowl/cubits/household_cubit.dart';

class SelectableButtonListTile extends StatefulWidget {
  final String title;
  final IconData? icon;
  final String? description;
  final bool selected;
  final bool raised;
  final void Function()? onPressed;
  final void Function()? onLongPressed;
  final Widget? extraOption;
  final Item item;
  final ShoppingList? shoppingList;

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
    required this.item,
    this.shoppingList,
  });

  @override
  State<SelectableButtonListTile> createState() =>
      _SelectableButtonListTileState();
}

class _SelectableButtonListTileState extends State<SelectableButtonListTile> {
  bool mouseHover = false;
  bool isEdited = false;
  TextEditingController controller = new TextEditingController();
  String description = "";
  late ItemEditCubit<Item> cubit;

  @override
  void initState() {
    super.initState();

    if (widget.item is ItemWithDescription) {
      controller.text = (widget.item as ItemWithDescription).description;
      description = (widget.item as ItemWithDescription).description;
    }
    cubit = ItemEditCubit<Item>(
      household: context
          .read<HouseholdCubit>()
          .state
          .household,
      item: widget.item,
      shoppingList: widget.shoppingList,
    );
  }

  @override
  void dispose() {
    cubit.close();
    controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final shoppingListCubit = BlocProvider.of<ShoppinglistCubit>(context);

    return Card(
      margin: const EdgeInsets.symmetric(vertical: 1),
      elevation: !widget.raised ? 0 : null,
      color: !widget.raised
          ? ElevationOverlay.applySurfaceTint(
        Theme
            .of(context)
            .colorScheme
            .background,
        Theme
            .of(context)
            .colorScheme
            .surfaceTint,
        1.5,
      )
          : Theme
          .of(context)
          .colorScheme
          .primary,
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
        child: Row(
          children: <Widget>[
            if (!isEdited)
              Expanded(
                child: ListTile(
                  dense: true,
                  leading: IconButton(
                    icon: !widget.raised ? Icon(Icons.add) : Icon(Icons.remove),
                    color: !widget.raised
                        ? null
                        : Theme
                        .of(context)
                        .colorScheme
                        .onPrimary
                        .withOpacity(.9),
                    onPressed: widget.onPressed,
                  ),
                  title: Text(
                    widget.title +
                        ((description.isNotEmpty ?? false)
                            ? (' - ' + description!)
                            : ''),
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    style: Theme
                        .of(context)
                        .textTheme
                        .bodyMedium!
                        .copyWith(
                        color: !widget.raised
                            ? null
                            : Theme
                            .of(context)
                            .colorScheme
                            .onPrimary
                            .withOpacity(.9)),
                  ),
                  selected: widget.selected,
                  visualDensity: VisualDensity(vertical: -4),
                  onTap: () {
                    setState(() {
                      isEdited = true;
                    });
                  },
                  onLongPress: widget.onLongPressed,
                  contentPadding: const EdgeInsets.only(left: 16, right: 8),
                  trailing: (widget.extraOption != null && mouseHover)
                      ? widget.extraOption
                      : (widget.onLongPressed != null && mouseHover)
                      ? IconButton(
                    onPressed: widget.onLongPressed,
                    color: widget.raised
                        ? Theme
                        .of(context)
                        .colorScheme
                        .onPrimary
                        : null,
                    icon: const Icon(Icons.more_horiz_rounded),
                  )
                      : null,
                ),
              ),
            if (isEdited)
              Expanded(
                child: Visibility(
                  child: TextField(
                    autofocus: true,
                    showCursor: true,
                    controller: controller,
                    decoration: InputDecoration(
                        border: InputBorder.none,
                        labelText: widget.title,
                        prefixIcon: IconButton(
                          icon: Icon(Icons.done),
                          onPressed: () async {
                            setState((){
                              isEdited = false;
                            });
                            await cubit.saveItem();
                            shoppingListCubit.refresh();
                          },
                        ),
                        contentPadding:
                        const EdgeInsets.only(left: 16, right: 8)),
                    onChanged: (s) => cubit.setDescription(s),
                    onSubmitted: (String value) async {
                      setState((){
                        isEdited = false;
                      });
                      await cubit.saveItem();
                      shoppingListCubit.refresh();
                    },
                    cursorColor: !widget.raised
                        ? null
                        : Theme
                        .of(context)
                        .colorScheme
                        .onPrimary
                        .withOpacity(.9),
                    style: Theme
                        .of(context)
                        .textTheme
                        .bodyMedium!
                        .copyWith(
                        color: !widget.raised
                            ? null
                            : Theme
                            .of(context)
                            .colorScheme
                            .onPrimary
                            .withOpacity(.9)),
                  ),
                ),
              ),
            Expanded(
              child: ListTile(
                dense: true,
                leading: null,
                selected: widget.selected,
                visualDensity: VisualDensity(vertical: -4),
                onTap: widget.onPressed,
                onLongPress: widget.onLongPressed,
                contentPadding: const EdgeInsets.only(left: 16, right: 8),
                trailing: null,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
