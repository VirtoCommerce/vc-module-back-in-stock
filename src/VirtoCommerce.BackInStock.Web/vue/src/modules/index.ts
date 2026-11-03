import * as blades from "./pages";
import * as locales from "./locales";
import { defineAppModule, registerExternalWidget, usePermissions } from "@vc-shell/framework";
import { markRaw } from "vue";
import SubscriptionsWidget from "./widgets/SubscriptionsWidget.vue";

// the AngularJS subscriptionWidget on customerDetail1: the contact's back in stock subscriptions
registerExternalWidget({
  id: "BackInStockSubscriptions",
  title: "Back in stock subscriptions",
  component: markRaw(SubscriptionsWidget),
  targetBlades: ["CustomerContactDetails"],
  // nothing to show for an unsaved contact, or without the permission the subscriptions blade and its search need
  isVisible: (blade) => !!blade?.param && usePermissions().hasAccess("BackInStock:subscription:read"),
});

export default defineAppModule({
  blades,
  locales,
});

export * from "./pages";
